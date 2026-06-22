import { authenticatedFetch } from "@/lib/authenticated-fetch";

async function fetchBookshelf(id: number) {
    const res = await authenticatedFetch(process.env.API_URL + `bookshelves/${id}`);

    console.log("Fetching bookshelf");
    console.log("Res:", res);
    const data = await res.json()
    console.log("Data:", data);
    return data;
}

export default async function BookshelfPage(props: PageProps<"/bookshelves/[id]">) {
    const { id } = await props.params;
    const bookshelfId = parseInt(id);
    const bookshelf = await fetchBookshelf(bookshelfId);
    return (
        <h1>Bookshelf {bookshelf.name}</h1>
    )
}