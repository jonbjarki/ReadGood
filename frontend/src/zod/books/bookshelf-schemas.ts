import { z } from "zod";
import { bookItemSchema } from "./books-schemas";


export const bookshelfListItemSchema = z.object({
    id: z.number(),
    name: z.string(),
    description: z.string().nullable()
});

export const bookshelfListBookItemSchema = z.object({
    id: z.number(),
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