import { addToBookshelfAction, fetchOwnBookshelvesAction } from "@/actions/bookshelf-actions";
import { auth } from "@/auth";
import { Button } from "../ui/button";
import { Link } from "lucide-react";
import { BookItem } from "@/types/books/books-search-response";
import AddToBookshelfDropdown from "./add-to-bookshelf-dropdown";

export default async function AddToBookshelfButton({ book, userName }: { book: BookItem, userName: string }) {

    const bookshelves = await fetchOwnBookshelvesAction(userName, book.id);
    const addBookToBookshelf = addToBookshelfAction.bind(null, book);

    return (
        <AddToBookshelfDropdown bookshelves={bookshelves} action={addBookToBookshelf} />
    )
}