import { addToBookshelfAction, fetchOwnBookshelvesAction } from "@/actions/bookshelf-actions";
import { auth } from "@/auth";
import { Button } from "../ui/button";
import { Link } from "lucide-react";
import { BookItem } from "@/types/books/books-search-response";
import AddToBookshelfDropdown from "./add-to-bookshelf-dropdown";

export default async function AddToBookshelfButton({ book }: { book: BookItem }) {
    const session = await auth();
    const user = session?.user;

    if (!user?.name) {
        return (
            <Link href="/api/auth/signin">
                <Button variant="outline">Add to bookshelf</Button>
            </Link>
        )
    }
    const bookshelves = await fetchOwnBookshelvesAction(user.name, book.id);
    const addBookToBookshelf = addToBookshelfAction.bind(null, book);

    return (
        <AddToBookshelfDropdown bookshelves={bookshelves} action={addBookToBookshelf} />
    )
}