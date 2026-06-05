import NextAuth from 'next-auth';
import { authConfig } from '../auth.config';
import Google from 'next-auth/providers/google';
import { cookies } from 'next/headers';

const API_URL = process.env.API_URL!;

type GoogleAuthResponse = {
    userId: string,
    email: string,
    userName?: string,
    jwtToken: string,
    expiresAt: Date
}




export const { handlers, auth, signIn, signOut } = NextAuth({
    ...authConfig,
    providers: [Google],
    session: {
        strategy: "jwt",
    },
    callbacks: {
        async jwt({ token, account, profile }) {
            if (!profile?.email) {
                throw new Error("Missing profile");
            }

            const idToken = account?.id_token;
            console.log("ID TOKEN=", idToken);

            // Authenticate with backend and creates the user if it does not exists
            // Backend returns custom JWT for authenticating future requests
            console.log("Making request to: ", API_URL + "auth/google")
            console.log("With body: ", JSON.stringify({ "idToken": idToken }));

            const res = await fetch(API_URL + "auth/google", {
                method: "POST",
                body: JSON.stringify({ idToken: idToken }),
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include"
            });
            console.log("RES:", res);

            if (!res.ok || res.status >= 400) {
                console.error("Failed to authenticate with backend", res.status, await res.text());
                return token;
            }

            const data = await res.json() as GoogleAuthResponse;

            /*             // Store the token in a cookie
                        const cookieStore = await cookies();
                        cookieStore.set({
                            name: "X-Access-Token",
                            value: data.jwtToken,
                            httpOnly: true,
                            secure: true,
                            sameSite: "lax",
                            path: "/",
                        }) */

            console.log("DATA=", data);
            token.accessToken = data.jwtToken;
            return token;
        },
        async session({ session, token }) {
            if (token) {
                session.user.accessToken = token.accessToken;
            }
            return session;
        }
    }
});