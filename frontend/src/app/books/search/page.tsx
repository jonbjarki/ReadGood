"use server"

import BooksList from "@/components/books-list/books-list";
import SearchBar from "@/components/search-bar";
import { BooksSearchResponse } from "@/types/API/books-search-response";
import { booksSearchResponseSchema } from "@/zod/books/books-schemas";
import { redirect } from "next/navigation";

// Server action to fetch search results for books based on a query string
async function searchBooksAction(title: string, author?: string, subject?: string): Promise<BooksSearchResponse> {
    let url = process.env.API_URL + "books/search?title=" + title;
    if (author) {
        url += "&author=" + author;
    }
    if (subject) {
        url += "&subject=" + subject;
    }
    const res = await fetch(url, { next: { revalidate: 120, tags: ["books-search"] } }); // cache results for 2 minutes

    if (!res.ok) {
        const errorText = await res.text();
        console.error("Failed to fetch books search results", res, errorText);
        switch (res.status) {
            case 400:
                throw new Error("Invalid search query");
            case 429:
                throw new Error("Too many requests. Please try again later.");
            default:
                throw new Error("Failed to fetch books search results: " + res.status + " " + errorText);
        }
    }

    const data = await res.json(); // raw data not yet typed or validated

    const validation = booksSearchResponseSchema.safeParse(data);
    if (!validation.success) {
        console.error("Invalid books search response format", data, validation.error);
        throw new Error("Invalid response format from server");
    }

    return validation.data; // Returns validated data with correct types
}

export default async function SearchPage({
    searchParams,
}: {
    searchParams?: Promise<{ [key: string]: string | string[] | undefined }>;
}) {
    const params = await searchParams;
    const title = params?.title?.toString();
    if (title === undefined) return redirect("/");

    const books = await searchBooksAction(title, params?.author?.toString(), params?.subject?.toString());

    return (
        <div className="flex flex-col gap-6">
            <SearchBar initialValue={title} />
            <BooksList items={books.results} />
        </div>
    )

}