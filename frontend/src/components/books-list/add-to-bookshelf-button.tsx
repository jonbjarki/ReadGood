import { fetchOwnBookshelvesAction, fetchUserBookshelvesAction } from "@/actions/bookshelf-actions";
import { auth } from "@/auth";
import { DropdownMenu, DropdownMenuContent, DropdownMenuGroup, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu";
import { Button } from "../ui/button";
import { authenticatedFetch, AuthenticationError } from "@/lib/authenticated-fetch";
import { redirect, unauthorized } from "next/navigation";
import { Link } from "lucide-react";

async function addToBookshelfAction(bookId: string, bookshelfId: number) {
    try {
        const res = await authenticatedFetch(process.env.API_URL + `bookshelves/${bookshelfId}/books/${bookId}`, {
            method: "POST"
        });
        if (!res.ok) {
            console.error("Error occurred when adding book to bookshelf");
            throw new Error("Error occurred when adding book to bookshelf");
        }
        console.log("Response:", res);
        console.log("Successfully added book to bookshelf");
    } catch (error) {
        if (error instanceof AuthenticationError) {

        }
    }
}

export default async function AddToBookshelfButton({ bookId }: { bookId: string }) {
    const session = await auth();
    const user = session?.user;

    if (!user?.name) {
        return (
            <Link href="/api/auth/signin">
                <Button variant="outline">Add to bookshelf</Button>
            </Link>
        )
    }
    const bookshelves = await fetchOwnBookshelvesAction(user.name);

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button variant="outline">Add to bookshelf</Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent>
                <DropdownMenuGroup>
                    {bookshelves.map((bookshelf) => (
                        <DropdownMenuItem key={bookshelf.id} >
                            <form action={async () => {
                                "use server"
                                await addToBookshelfAction(bookId, bookshelf.id)
                            }}>
                                <button type="submit">{bookshelf.name}</button>
                            </form>
                        </DropdownMenuItem>
                    ))}
                </DropdownMenuGroup>
            </DropdownMenuContent>
        </DropdownMenu>
    )
}