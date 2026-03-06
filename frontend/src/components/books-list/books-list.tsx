import { BookSearchItem } from "@/types/API/books-search-response";
import BooksListItem from "./books-list-item";

export default function BooksList({ items }: { items: BookSearchItem[] }) {
    return (
        <ul className="ml-4 flex flex-col gap-6">
            {items.map((item) => (
                <BooksListItem key={item.id} item={item} />
            ))}
        </ul>
    )
}