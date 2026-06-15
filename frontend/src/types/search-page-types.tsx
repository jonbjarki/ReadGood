import { searchPageParamsSchema } from "@/zod/books/books-schemas";
import z from "zod";

export type SearchPageParamsType = z.infer<typeof searchPageParamsSchema>;