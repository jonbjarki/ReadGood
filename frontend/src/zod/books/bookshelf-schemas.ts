import { z } from "zod";
import { bookItemSchema } from "./books-schemas";


export const bookshelfListItemSchema = z.object({
    id: z.number(),
    name: z.string(),
    description: z.string().nullable()
});

export const bookshelfDetailsSchema = z.object({
    id: z.number(),
    name: z.string(),
    description: z.string().nullable(),
    books: z.array(bookItemSchema)
});

export const bookshelfListResponseSchema = z.array(bookshelfListItemSchema);