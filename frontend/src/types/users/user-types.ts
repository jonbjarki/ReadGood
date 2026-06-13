import { userProfileSchema } from "@/zod/books/user-schemas";
import { z } from "zod";

export type UserProfileResponse = z.infer<typeof userProfileSchema>;