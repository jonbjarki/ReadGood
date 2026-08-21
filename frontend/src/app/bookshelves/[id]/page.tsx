import { authenticatedFetch } from "@/lib/authenticated-fetch";
import { bookshelfDetailsSchema } from "@/zod/books/bookshelf-schemas";
import Link from "next/link";

async function fetchBookshelf(id: number) {
    const res = await authenticatedFetch(process.env.API_URL + `bookshelves/${id}`);

    console.log("Fetching bookshelf");
    const unvalidated = await res.json();
    const validation = bookshelfDetailsSchema.safeParse(unvalidated);
    console.log("Unvalidated:", unvalidated);

    if (!validation.success) {
        console.error("Validation error in fetch bookshelf", validation.error);
        throw new Error("Something went wrong when validating bookshelf response");
    }

    const data = validation.data;
    console.log("Data:", data);
    return data;
}

export default async function BookshelfPage(props: PageProps<"/bookshelves/[id]">) {
    const { id } = await props.params;
    const bookshelfId = parseInt(id);
    const bookshelf = await fetchBookshelf(bookshelfId);
    return (
        <main>
            <h2>{bookshelf.name}</h2>
            <ul>
                {bookshelf.books.map(book => (
                    <li key={book.id}>
                        <Link href={`/books/${book.id}`}>{book.title}</Link>
                    </li>
                ))}
            </ul>

        </main>

    )
}