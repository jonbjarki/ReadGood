import z from "zod";

export const searchPageParamsSchema = z.object({
    title: z.string().min(1, "Title is required"),
    author: z.string().optional(),
    subject: z.string().optional(),
    page: z.coerce.number().int().positive().default(1) // Zod will attempt to parse the page parameter as a positive integer
});