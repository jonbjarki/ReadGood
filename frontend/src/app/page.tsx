import SearchBar from "@/components/search-bar";
import { redirect } from "next/navigation";
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