"use client"

import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuTrigger } from "../ui/dropdown-menu"
import { Button } from "../ui/button"
import { BookshelfListItem } from "@/types/bookshelves/bookshelf-types"
import { BookItem } from "@/types/books/books-search-response"
import { useState, useTransition } from "react"
import { bookshelfListItemSchema } from "@/zod/books/bookshelf-schemas"

export default function AddToBookshelfDropdown({
    bookshelves,
    action,
}: {
    bookshelves: BookshelfListItem[]
    action: (bookshelfId: number) => Promise<void>
}) {
    const [isPending, startTransition] = useTransition();
    const [addedTo, setAddedTo] = useState<BookshelfListItem["id"][]>([])
    const isDisabled = (bookshelf: BookshelfListItem) => !!bookshelf.isBookInShelf || !!addedTo.some(id => id == bookshelf.id) || !!isPending;


    const handleClick = (bookshelfId: BookshelfListItem["id"]) => {
        setAddedTo(prev => [...prev, bookshelfId])
        startTransition(() => {
            action(bookshelfId);
        })
    }
    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button variant="outline">Add to bookshelf</Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
                <DropdownMenuGroup>
                    {bookshelves.map((bookshelf) => (
                        <DropdownMenuItem key={bookshelf.id} className="w-56" disabled={isDisabled(bookshelf)}>
                            <button onClick={() => {
                                handleClick(bookshelf.id);
                            }} disabled={isDisabled(bookshelf)}>{bookshelf.name}</button>
                        </DropdownMenuItem>
                    ))}
                </DropdownMenuGroup>
            </DropdownMenuContent>
        </DropdownMenu>
    )
}