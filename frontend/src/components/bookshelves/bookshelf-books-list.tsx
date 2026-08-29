import { authenticatedFetch } from "@/lib/authenticated-fetch";
import { BookshelfBookItem, BookshelfBooksPagingParams, BookshelfBooksResponse } from "@/types/bookshelves/bookshelf-types";
import { bookshelfBooksItem, bookshelfBooksResponseSchema } from "@/zod/books/bookshelf-schemas";
import BookshelfBook from "./bookshelf-book";

async function fetchBookshelfBooks(params: BookshelfBooksPagingParams) {
    const url = new URL(process.env.API_URL + `bookshelves/`);
    const { page } = params;
    url.searchParams.append("page", page);
    const unvalidated = await authenticatedFetch(process.env.API_URL + `bookshelves/`);
    const res = await bookshelfBooksResponseSchema.safeParseAsync(unvalidated);
    if (!res.success) {
        throw new Error("Unexpected response received from server");
    }

    return res.data as BookshelfBooksResponse;
}

export default async function BookshelfBooksList({ bookshelfId, params }: { bookshelfId: number, params: BookshelfBooksPagingParams }) {
    const res = await fetchBookshelfBooks(params);
    const books = res.results;

    return ({
        books.map(book: BookshelfBooksItem => (
            <li key={book.id}>
                <BookshelfBook book={book} />
            </li>
        ))
    })
}