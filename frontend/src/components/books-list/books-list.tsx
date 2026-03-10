import { BookSearchItem } from "@/types/API/books-search-response";
import BooksListItem from "./books-list-item";
import PaginationControls from "./pagination-controls";
import { SearchPageParamsType } from "@/types/search-page-types";

export default function BooksList({ items, parsedParams, totalPages }: { items: BookSearchItem[], parsedParams: SearchPageParamsType, totalPages: number}) {
    return (
        <>
        <ul className="ml-4 flex flex-col gap-6">
            {items.map((item) => (
                <BooksListItem key={item.id} item={item} />
            ))}
        </ul>
            <PaginationControls parsedParams={parsedParams} totalPages={totalPages} />
        </>
    )
}