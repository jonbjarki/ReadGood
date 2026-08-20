import { auth } from "@/auth";
import AvatarImage from "@/components/profile/avatar-image";
import ProfileBookshelves from "@/components/profile/profile-bookshelves";
import { Button } from "@/components/ui/button";
import { UserProfileResponse } from "@/types/users/user-types";
import { userProfileSchema } from "@/zod/users/user-schemas";
import { Suspense } from "react";


async function fetchUserProfile(username: string): Promise<UserProfileResponse> {
    "use server"

    const res = await fetch(process.env.API_URL + `users/${username}`);
    if (!res.ok) {
        throw new Error(`Fetching user profile failed with status code ${res.status}`);
    }
    const unvalidated = await res.json();

    const validation = await userProfileSchema.safeParseAsync(unvalidated);
    if (!validation.success) {
        console.error("Validation failed", validation);
        throw new Error("Validation failed when fetching user profile", validation.error);
    }

    return validation.data;
}

export default async function ProfilePage(props: PageProps<'/users/[username]/profile'>) {
    const session = await auth();
    const { username } = await props.params;
    const isCurrentUser = !!username && session?.user.name === username;

    const user = await fetchUserProfile(username);
    const dateJoined = user.dateJoined.toLocaleDateString();

    return (
        <main>
            <div className="flex flex-row flex-nowrap gap-8 text-center w-fit mb-10">
                <div className="flex flex-col gap-4">
                    <AvatarImage
                        src={user.imageUrl ?? undefined}
                        name={user.userName ?? undefined}
                        size="large"
                    />
                    <h2 className="text-3xl font-bold">{user.userName}</h2>
                </div>
                <div className="flex flex-col gap-2 justify-center items-start">
                    <p>Joined: {dateJoined}</p>
                    {/* <span className="w-full flex flex-row gap-2">
                        <b className="font-bold">x</b> <p>followers</p>
                        <b className="font-bold inline">x</b> <p>following</p>
                    </span>
                    <Button size="lg">Follow</Button> */}
                </div>
            </div>

            <Suspense fallback="Loading...">
                <ProfileBookshelves username={user.userName} isCurrentUser={isCurrentUser} />
            </Suspense>

        </main>
    )
}