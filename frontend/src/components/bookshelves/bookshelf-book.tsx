import { BookshelfBookItem, BookshelfListItem } from "@/types/bookshelves/bookshelf-types";
import Link from "next/link";
import Image from "next/image"
import CoverImageWithFallback from "../books-list/image-with-fallback";

export default function BookshelfBook({ book }: { book: BookshelfBookItem }) {
    return (
        <Link href={"/books/" + book.id} className="flex flex-row gap-4 items-center justify-left">
            <div className="relative w-32 h-48">
                <CoverImageWithFallback title={book.title ?? ""} url={book.coverImageUrl} />
            </div>
            <div className="flex flex-col gap-4">
                <h3 className="text-sm lg:text-lg font-semibold">{book.title}</h3>
            </div>
        </Link>
    )
}