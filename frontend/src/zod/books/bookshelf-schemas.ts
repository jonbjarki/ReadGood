import { z } from "zod";
import { bookItemSchema } from "./books-schemas";


export const bookshelfListItemSchema = z.object({
    id: z.number(),
    name: z.string(),
    description: z.string().nullable(),
    isBookInShelf: z.boolean().nullable() // Shows whether provided book is already in the bookshelf. This is only included when fetching user's own bookshelves with a bookId query parameter.
});

export const bookshelfListBookItemSchema = z.object({
    id: z.string(),
    title: z.string(),
    coverImageUrl: z.url().nullable()
});

export const bookshelfDetailsSchema = z.object({
    id: z.number(),
    name: z.string(),
    description: z.string().nullish(),
    books: z.array(bookshelfListBookItemSchema)
});

export const bookshelfListResponseSchema = z.array(bookshelfListItemSchema);