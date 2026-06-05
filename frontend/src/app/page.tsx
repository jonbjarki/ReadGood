import { auth, signOut, signIn } from "@/auth";
import SearchBar from "@/components/search-bar";
import { authenticatedFetch } from "@/lib/authenticated-fetch";
import { StrictMode } from "react";

// Allows self-signed certificates for development only
if (process.env.NODE_ENV == "development") {
    process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';
}


async function whoAmI() {
  "use server"
  const res = await authenticatedFetch(process.env.API_URL + "auth/me", {
    method: "GET",
    credentials: "include"
  });

  console.log(res);
  const data = await res.text();
  console.log(data);
  return data;
}

export default async function HomePage() {
  const session = await auth();
  const user = session?.user;
  console.log("SESSION=", session);

  return (
    <StrictMode>
      <main>
        <form
          action={async () => {
            "use server"
            await signOut()
          }}
        >
          <button type="submit">Sign Out</button>
        </form>
        <form
          action={async () => {
            "use server"
            await signIn()
          }}
        >
          <button type="submit">Sign In</button>
        </form>
        <SearchBar />
        {user && (
          <>
            <div className="mt-4 text-sm text-gray-600">
              Signed in as {user.email}
            </div>
            <form action={async () => {
              "use server"
              const data = await whoAmI();
              console.log("WHOAMI=", data);
            }}
            >
              <button type="submit" className="mt-2 px-4 py-2 bg-blue-500 text-white rounded">
                Who Am I?
              </button>
            </form>
          </>
        )}
      </main>
    </StrictMode>
  )
}