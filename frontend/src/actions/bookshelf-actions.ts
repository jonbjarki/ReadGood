"use server";

import { authenticatedFetch, AuthenticationError } from "@/lib/authenticated-fetch";
import { BookItem } from "@/types/books/books-search-response";
import { bookshelfListResponseSchema } from "@/zod/books/bookshelf-schemas";

export async function fetchUserBookshelvesAction(username: string) {
    const res = await authenticatedFetch(process.env.API_URL + `bookshelves/user/${username}`);
    if (!res.ok) {
        console.error(`Request to fetch user bookshelves failed with status: ${res.status} ${await res.text()}`)
        throw new Error("Request to fetch user bookshelves failed");
    }

    const unvalidated = await res.json();
    const validation = await bookshelfListResponseSchema.safeParseAsync(unvalidated);
    if (!validation.success) {
        console.error("Validation failed when fetching user bookshelves", validation.error)
        throw new Error("Validation failed when fetching user bookshelves");
    }
    console.log("Fetched user's bookshelves:", validation.data);
    return validation.data;

}

export async function fetchOwnBookshelvesAction(username: string, bookId?: string) {
    // If bookId is provided, include it as a query parameter to check if the book is already in the bookshelf
    const path = process.env.API_URL + `bookshelves/user/${username}${bookId ? `?bookId=${bookId}` : ""}`;
    const res = await authenticatedFetch(path, {
        cache: "force-cache",
        next: {
            revalidate: 120,
            tags: ["user-bookshelves"]
        }
    });

    if (!res.ok) {
        console.error(`Request to fetch user bookshelves failed with status: ${res.status} ${await res.text()}`)
        throw new Error("Request to fetch user bookshelves failed");
    }


    const unvalidated = await res.json();
    const validation = await bookshelfListResponseSchema.safeParseAsync(unvalidated);
    if (!validation.success) {
        console.error("Validation failed when fetching user bookshelves", validation.error)
        throw new Error("Validation failed when fetching user bookshelves");
    }
    console.log("Fetched user's bookshelves:", validation.data);
    return validation.data;
}

export async function addToBookshelfAction(book: BookItem, bookshelfId: number) {
    try {
        const res = await authenticatedFetch(process.env.API_URL + `bookshelves/${bookshelfId}/books/${book.id}`, {
            method: "POST",
            body: JSON.stringify({
                title: book.title,
                thumbnailUrl: book.coverImageUrl
            }),
            headers: new Headers({ "Content-Type": "application/json" })
        });
        if (!res.ok) {
            console.error("Error occurred when adding book to bookshelf");
            let error = await res.text()
            console.error(error);
            throw new Error("Error occurred when adding book to bookshelf");
        }
        console.log("Response:", res);
        console.log("Successfully added book to bookshelf");
    } catch (error) {
        if (error instanceof AuthenticationError) {

        }
    }
}