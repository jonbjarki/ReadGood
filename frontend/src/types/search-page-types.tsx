import { searchPageParamsSchema } from "@/zod/search-page-schemas";
import z from "zod";

export type SearchPageParamsType = z.infer<typeof searchPageParamsSchema>;