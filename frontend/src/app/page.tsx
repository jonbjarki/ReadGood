import SearchBar from "@/components/search-bar";
import { StrictMode } from "react";

export default async function HomePage() {
  return (
    <StrictMode>
      <main className="dark:bg-black">
        <SearchBar />
      </main>
    </StrictMode>
  )
}