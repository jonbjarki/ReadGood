import NextAuth from 'next-auth';
import { authConfig } from '../auth.config';
import Google from 'next-auth/providers/google';

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
  callbacks: {
     async signIn({ account, profile }) {
            if (!profile?.email) {
                throw new Error("Missing profile");
            }

            const idToken = account?.id_token;
            console.log("ID TOKEN=",idToken);

            // Authenticate with backend to create user if it does not exist
            // Returns the application JWT
            console.log("Making request to: ", API_URL + "auth/google")
            console.log("With body: ", JSON.stringify({ "idToken": idToken }));

            const res = await fetch(API_URL + "auth/google", {
                method: "POST",
                body: JSON.stringify({ idToken: idToken }),
                headers: {
                  "Content-Type": "application/json"
                }
            });
            
            console.log("RES:", res);
            
            const data = await res.json() as GoogleAuthResponse;
            console.log("DATA=",data);
            return true;
        },
  }
});