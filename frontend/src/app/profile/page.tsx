import { auth } from "@/auth";

export default async function ProfilePage() {
    const session = await auth();
    if (!session)
        return null;

    const { user } = session;

    return (
        <>
            <h1>Your Profile</h1>
            <p>Name: {user.name}</p>
            <p>Email: {user.email}</p>
        </>
    )
}