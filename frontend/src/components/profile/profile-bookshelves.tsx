import { authenticatedFetch } from "@/lib/authenticated-fetch";
import { bookshelfListResponseSchema } from "@/zod/books/bookshelf-schemas";

async function fetchUserBookshelves(username: string) {

    const res = await authenticatedFetch(process.env.API_URL + `bookshelves/user/${username}`);
    if (!res.ok) {
        console.error(`Request to fetch user bookshelves failed with status: ${res.status} ${await res.text()}`)
        throw new Error("Request to fetch user bookshelves failed");
    }

    const unvalidated = await res.json();
    const validation = await bookshelfListResponseSchema.safeParseAsync(unvalidated);
    if (!validation.success) {
        console.error("Validation failed when fetching user bookshelves", validation.error)
        throw new Error("Validation failed when fetching user bookshelves");
    }
    console.log("Fetched user's bookshelves:", validation.data);
    return validation.data;

}

export default async function ProfileBookshelves({ username }: { username: string }) {
    const bookshelves = await fetchUserBookshelves(username);

    return (
        <ul>
            {bookshelves.map(item => (
                <li key={item.id}>
                    {item.name}
                </li>
            ))}
        </ul>
    )
}