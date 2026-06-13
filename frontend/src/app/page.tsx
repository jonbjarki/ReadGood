import { auth, signOut, signIn } from "@/auth";
import SearchBar from "@/components/search-bar";
import { StrictMode } from "react";

export default async function HomePage() {
  return (
    <StrictMode>
      <main>
        <SearchBar />
      </main>
    </StrictMode>
  )
}