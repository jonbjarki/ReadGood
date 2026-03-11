import { BookSearchItem } from "@/types/API/books-search-response";
import BooksListItem from "./books-list-item";
import PaginationControls from "./pagination-controls";
import { SearchPageParamsType } from "@/types/search-page-types";


type BooksListProps = {
    items: BookSearchItem[];
    parsedParams: SearchPageParamsType;
    hasNext: boolean;
    hasPrevious: boolean;
}

export default function BooksList({ items, parsedParams, hasNext, hasPrevious }: BooksListProps) {
    return (
        <>
            <ul className="ml-4 flex flex-col gap-6">
                {items.length === 0 && parsedParams.page === 1 && (
                    <li>No results found.</li>
                )}
                {items.length === 0 && parsedParams.page > 1 && (
                    <li>No more results found.</li>
                )}
                {items.map((item) => (
                    <BooksListItem key={item.id} item={item} />
                ))}
            </ul>
            <PaginationControls parsedParams={parsedParams} itemsEmpty={items.length === 0} hasNext={hasNext} hasPrevious={hasPrevious} />
        </>
    )
}