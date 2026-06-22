import { z } from "zod";
import { bookshelfDetailsSchema, bookshelfListItemSchema } from "@/zod/books/bookshelf-schemas";

export type BookshelfListItem = z.infer<typeof bookshelfListItemSchema>;
export type BookshelfDetails = z.infer<typeof bookshelfDetailsSchema>;