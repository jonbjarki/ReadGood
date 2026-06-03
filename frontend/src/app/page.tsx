import { auth, signOut, signIn } from "@/auth";
import SearchBar from "@/components/search-bar";
import { StrictMode } from "react";

async function whoAmI() {
  const res = await fetch(process.env.API_URL + "auth/me", {
    method: "GET",
    credentials: "include"
  });
  console.log(res);
  const data = await res.text();
  console.log(data);
}

export default async function HomePage() {
  const session = await auth();
  const user = session?.user;

  const whoami = await whoAmI();

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
            You are {whoami}
          </>
        )}
      </main>
    </StrictMode>
  )
}