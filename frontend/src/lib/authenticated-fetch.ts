import { auth } from "@/auth";
import { request } from "http";
import { decode, getToken } from "next-auth/jwt";
import { cookies } from "next/headers";

class AuthenticationError extends Error {
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
    if (!accessToken) {
        throw new AuthenticationError("User is not authenticated");
    }

    // Make the authenticated request to the backend API, including the JWT in the Authorization header
    return await fetch(input, {
        ...init,
        headers: {
            ...init?.headers,
            "Authorization": `Bearer ${accessToken}`,
        }
    });

}
