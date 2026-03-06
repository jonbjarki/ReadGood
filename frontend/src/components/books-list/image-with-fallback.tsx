import Image from 'next/image';
import { AspectRatio } from '@/components/ui/aspect-ratio';

interface ImageWithFallbackProps {
    title: string;
    url: string | null;
}



export default function CoverImageWithFallback({
    title,
    url,
}: ImageWithFallbackProps) {
    if (url) {

        return (
            <Image
                src={url}
                alt={`Book cover for ${title}`}
                fill
                loading='lazy'
                className='rounded object-cover w-auto h-auto'
            />
        );
    }

    return (
        <div className={`flex items-center justify-center bg-gray-200 rounded w-full h-full`}>
            <p className="text-center text-gray-700 px-4 text-sm font-semibold">
                {title}
            </p>
        </div>
    );
}