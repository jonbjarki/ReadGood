import Link from "next/link";
import ThemeToggle from "./theme/theme-toggle";
import AvatarImage from "./profile/avatar-image";
import { auth, signIn, signOut } from "@/auth";

async function handleSignIn() {
    "use server"
    await signIn();
}

async function handleSignOut() {
    "use server"
    await signOut();
}


export default async function Header() {
    const session = await auth();
    return (

        <header className="w-full border-b py-4 px-4 mb-8 flex justify-center items-center">
            <div className="w-3xl flex items-center justify-between">
                <Link href="/"><h1 className="text-2xl font-medium text-primary">ReadTogether</h1></Link>
                <div className="flex flex-row gap-2 items-center">
                    {!session ? (
                        <form className="text-center h-fit" action={handleSignIn}>
                            <button type="submit">Sign In</button>
                        </form>
                    ) :
                        (
                            <form action={handleSignOut}>
                                <button type="submit">Sign Out</button>
                            </form>
                        )}
                    {session &&

                        <Link href={`/users/${session.user.name?.toLowerCase()}/profile`}>
                            <AvatarImage
                                src={session.user.image ?? undefined}
                                name={session.user.name ?? undefined}
                                size="small"
                            />
                        </Link>
                    }
                    <ThemeToggle />
                </div>
            </div>
        </header>
    )
}