import CoverImageWithFallback from "@/components/books-list/image-with-fallback";
import type { BookItem } from "@/types/API/books-search-response";
import DOMPurify from 'dompurify';
import parse from 'html-react-parser';
import { notFound } from "next/navigation";
import { JSDOM } from "jsdom"

async function fetchBookAction(bookId: string): Promise<BookItem> {
    const res = await fetch(process.env.API_URL + "books/" + bookId, { next: { revalidate: 300 } }); // cache for 5 minutes

    if (!res.ok) {
        if (res.status === 404) {
            return notFound();
        }
        const errorContent = await res.text();
        console.error("error occured fetching book", res.status, errorContent);
        throw new Error("Failed to fetch book details");
    }

    return await res.json() satisfies BookItem;
}

export default async function BookPage({ params }: { params: Promise<{ id: string }> }) {
    const id = (await params).id;

    const book = await fetchBookAction(id);

    // Sanitizes and parses description as there are often simple HTML tags included "<p> <b> <i> etc."
    const window = new JSDOM('').window;
    const purify = DOMPurify(window);
    const sanitizedDescription = purify.sanitize(book.description ?? "")
    const description = parse(sanitizedDescription);

    return (
        <main className="px-4 py-8 max-w-3xl mx-auto">
            <div className="flex flex-col md:flex-row gap-8">
                <div className="shrink w-full md:w-48">
                    <div className="relative w-full h-0 pb-[150%] md:pb-[150%]">
                        <CoverImageWithFallback url={book.coverImageUrl} title={book.title} />
                    </div>
                </div>

                {/* main content */}
                <div className="flex-1">
                    <h1 className="text-2xl font-bold">{book.title}</h1>
                    <div className="mt-4 space-y-1 text-sm text-gray-600">
                        {book.authorName && (
                            <p>
                                By {book.authorName} {book.firstPublishedYear && ("· " + book.firstPublishedYear)}
                            </p>
                        )}
                    </div>
                    <div className="text-balance whitespace-pre-wrap leading-relaxed mt-4 max-h-40 overflow-y-scroll">
                        {description}
                    </div>
                </div>
            </div>
        </main>
    );
}