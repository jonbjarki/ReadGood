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
    hasNext: z.boolean(),
    hasPrevious: z.boolean(),
})

export const searchPageParamsSchema = z.object({
    title: z.string().min(1, "Title is required"),
    author: z.string().optional(),
    subject: z.string().optional(),
    page: z.coerce.number().int().positive().default(1) // Zod will attempt to parse the page parameter as a positive integer
});

export const bookItemSchema = z.object({
    id: z.string(),
    title: z.string(),
    description: z.string().nullable(),
    firstPublishedYear: z.string().nullable(),
    authorName: z.string().nullable(),
    coverImageUrl: z.string().nullable(),
});

