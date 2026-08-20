import { userProfileSchema } from "@/zod/users/user-schemas";
import { z } from "zod";

export type UserProfileResponse = z.infer<typeof userProfileSchema>;