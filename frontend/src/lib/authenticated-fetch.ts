import { auth } from "@/auth";
import { getToken } from "next-auth/jwt";
import { cookies } from "next/headers";

class AuthenticationError extends Error {
    constructor(message: string) {
        super(message);
        this.name = "AuthenticationError";
    }
}

// Utility function to make authenticated requests to the backend API
export async function authenticatedFetch(input: URL | RequestInfo, init?: RequestInit): Promise<Response> {
    "use server"
    const session = await auth();
    if (!session) {
        throw new AuthenticationError("User is not authenticated");
    }
    const token = session.user.accessToken;
    if (!token) {
        throw new AuthenticationError("No access token found in session");
    }

    const headers = new Headers(init?.headers);
    headers.set("Authorization", `Bearer ${token}`);
    const response = await fetch(input, { ...init, headers });
    return response;
};
