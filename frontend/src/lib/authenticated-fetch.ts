import { auth } from "@/auth";
import { request } from "http";
import { decode, getToken } from "next-auth/jwt";
import { redirect, RedirectType } from 'next/navigation'
import { cookies } from "next/headers";

export class AuthenticationError extends Error {
    constructor(message: string) {
        super(message);
        this.name = "AuthenticationError";
    }
}

async function getDecodedToken() {
    // Retrieve the encoded authjs session token from cookies
    const cookieStore = await cookies();

    // The default cookie name for https-only session tokens.
    const cookieName = "__Secure-authjs.session-token"
    const sessionCookie = cookieStore.get(cookieName)?.value;

    if (!sessionCookie) return null;

    // Decode the session cookie to extract the JWT token
    const decodedToken = await decode({
        token: sessionCookie,
        secret: process.env.AUTH_SECRET!,
        salt: cookieName, // Use the cookie name as salt for decoding
    });

    if (!decodedToken) {
        return null;
    }

    return decodedToken.accessToken;
}

/**
Utility function for making authenticated requests to the backend API.
It retrieves the JWT from the encoded session cookie and includes it in the Authorization header of the request. 
*/
export async function authenticatedFetch(input: URL | RequestInfo, init?: RequestInit) {
    const accessToken = await getDecodedToken();
    const headers = new Headers(init?.headers || {});
    if (accessToken) {
        // If we have a valid access token, include it in the Authorization header
        headers.set("Authorization", `Bearer ${accessToken}`);
    }

    // Make the authenticated request to the backend API, including the JWT in the Authorization header if available
    const res = await fetch(input, {
        ...init,
        headers
    });

    if (res.status === 401) {
        // If the response status is 401 (Unauthorized), redirect to the sign-in page
        redirect('/api/auth/signin', RedirectType.replace);
    }

    return res;
}
