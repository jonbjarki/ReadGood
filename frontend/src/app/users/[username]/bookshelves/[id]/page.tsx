import { authenticatedFetch } from "@/lib/authenticated-fetch";

async function fetchBookshelf(id: number) {
    const res = await authenticatedFetch(process.env.API_URL + `bookshelves/${id}`);

    console.log("Fetching bookshelf");
    console.log("Res:", res);
    console.log("Data:", await res.json());
}

export default async function BookshelfPage(props: PageProps<"/users/[username]/bookshelves/[id]">) {
    const { id } = await props.params;
    try {
        const bookshelfId = parseInt(id);
        await fetchBookshelf(bookshelfId);

    } catch (e) {
        console.error(e);
    }
}