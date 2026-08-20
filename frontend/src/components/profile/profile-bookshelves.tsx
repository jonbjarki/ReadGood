import { fetchUserBookshelvesAction } from "@/actions/bookshelf-actions";
import Link from "next/link";

export default async function ProfileBookshelves({ username, isCurrentUser }: { username: string, isCurrentUser: boolean }) {
    const bookshelves = await fetchUserBookshelvesAction(username);

    return (
        <>
            <h3 className="mb-1 pr-1 w-full flex justify-between">
                <p>━ {username}'s Bookshelves</p>

                {/* isCurrentUser && <Link href="/">Edit</Link> */}
            </h3>
            <ul className="flex flex-row flex-wrap px-2 py-2 gap-2 border border-foreground rounded-sm">
                {bookshelves.map(shelf => (
                    <li key={shelf.id} className="border m-0 px-1 border-foreground/50 hover:bg-accent text-sm cursor-pointer">
                        <Link href={`/bookshelves/${shelf.id}`}>{shelf.name.toLocaleLowerCase()}</Link>
                    </li>
                ))}
            </ul>
        </>
    )
}