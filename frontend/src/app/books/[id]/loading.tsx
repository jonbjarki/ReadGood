import CoverImageWithFallback from "@/components/books-list/image-with-fallback";
import { Button } from "@/components/ui/button";

export default function Loading() {
    return (
        <main className="px-4 py-8 max-w-3xl mx-auto">
            <div className="flex flex-col md:flex-row gap-8 mb-8 items-center">
                <div className="shrink w-full md:w-48">
                    <div className="relative w-full h-0 pb-[150%] md:pb-[150%]">
                        <CoverImageWithFallback url={null} title="" />
                    </div>
                </div>

                <div className="flex flex-col shrink-2 basis-0 grow items-start gap-4">
                    <h1 className="text-2xl font-bold">...</h1>
                    <div className="space-y-1 text-lg">
                        <p>...</p>
                    </div>



                    <Button variant="outline">...</Button>
                </div>
            </div>
            <p className="leading-7 text-pretty">
                ...
            </p>
        </main>
    )
}