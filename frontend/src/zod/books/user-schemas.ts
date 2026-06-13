import { z } from "zod";

export const userProfileSchema = z.object({
    userName: z.string(),
    imageUrl: z.url().optional(),
    dateJoined: z.coerce.date()
})