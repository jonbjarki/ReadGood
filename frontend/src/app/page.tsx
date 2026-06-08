import { auth, signOut, signIn } from "@/auth";
import SearchBar from "@/components/search-bar";
import { StrictMode } from "react";

// Allows self-signed certificates for development only
if (process.env.NODE_ENV == "development") {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}

export default async function HomePage() {
  const session = await auth();
  return (
    <StrictMode>
      <main>
        {session ? (
          <>
          <p>Signed in as {session.user.email}</p>
          <form
          action={async () => {
            "use server"
            await signOut()
          }}
        >
          <button type="submit">Sign Out</button>
        </form>
          </>
        ) : (
          <p>Not signed in</p>
        )}
        <form
          action={async () => {
            "use server"
            await signIn()
          }}
        >
          <button type="submit">Sign In</button>
        </form>
        <SearchBar />
      </main>
    </StrictMode>
  )
}