import {z} from "zod";

export const bookSearchItemSchema = z.object({
    id: z.string(),
    title: z.string(),
    coverImageUrl: z.string().nullable(),
    author: z.string().nullable(),
    firstPublished: z.string().nullable(),
})

export const booksSearchResponseSchema = z.object({
    results: z.array(bookSearchItemSchema),
    page: z.number(),
    pageSize: z.number(),
    total: z.number(),
})


export const bookItemSchema = z.object({
    title: z.string(),
    description: z.string().nullable(),
    firstPublished: z.string().nullable(),
    authorName: z.string().nullable(),
    coverImageUrl: z.string().nullable(),
});