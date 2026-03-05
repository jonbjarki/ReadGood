"use client";

import { BookSearchItem } from "@/types/API/books-search-response";
import ImageWithFallback from "./image-with-fallback";
import Link from "next/link";

function getSubtitle(author: string | null, firstPublished: string | null) {
    if (author && firstPublished) {
        return `${author} • ${firstPublished}`;
    }
    else if (author) {
        return author;
    }
    else if (firstPublished) {
        return firstPublished;
    }

    return null;
}



export default function BooksListItem({ item }: { item: BookSearchItem }) {
    const subtitle = getSubtitle(item.author, item.firstPublished);

    return (
        <li>
            <Link href={"/books/" + item.id} className="flex flex-row gap-4 items-center">
                <ImageWithFallback title={item.title} url={item.coverImageUrl} />
                <div className="flex flex-col gap-4">
                    <h3 className="text-sm lg:text-lg font-semibold">{item.title}</h3>
                    <p className="text-gray-600 text-xs lg:text-sm">{subtitle}</p>
                </div>
            </Link>
        </li>
    )
}