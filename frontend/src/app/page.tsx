import { auth } from "@/auth";
import SearchBar from "@/components/search-bar";
import { StrictMode } from "react";

export default async function HomePage() {
  const session = await auth();
  const user = session?.user;
  
  return (
    <StrictMode>
      <main>
        <SearchBar />
        {user && (
          <div className="mt-4 text-sm text-gray-600">
            Signed in as {user.email}
          </div>
        )}
      </main>
    </StrictMode>
  )
}