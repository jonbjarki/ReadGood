import { handlers } from "@/auth";

// Allows self-signed certificates for development only
if (process.env.NODE_ENV == "development") {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}
export const { GET, POST } = handlers;