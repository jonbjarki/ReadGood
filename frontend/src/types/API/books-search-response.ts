import { bookSearchItemSchema, booksSearchResponseSchema } from "@/zod/books/books-schemas";
import z from "zod";

export type BookSearchItem = z.infer<typeof bookSearchItemSchema>;
export type BooksSearchResponse = z.infer<typeof booksSearchResponseSchema>;
