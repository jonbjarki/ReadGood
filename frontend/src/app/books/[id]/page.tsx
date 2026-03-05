export default function BookPage({ params }: { params: { id: string } }) {
    return (
        <div>
            <h1>Book {params.id}</h1>
        </div>
    )
}