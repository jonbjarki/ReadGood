import { NextAuthConfig } from "next-auth";

export const authConfig = {
    providers: [],
    callbacks: {
        authorized({ auth, request: { nextUrl } }) {
            const isLoggedIn = !!auth?.user;
            // Here you can read the current path and redirect users away if unauthorized.
        }
    }
} satisfies NextAuthConfig