import Image from 'next/image';
import { AspectRatio } from '@/components/ui/aspect-ratio';

interface ImageWithFallbackProps {
    title: string;
    url: string | null;
}



export default function ImageWithFallback({
    title,
    url
}: ImageWithFallbackProps) {
    if (url) {

        return (
            <div className='relative w-32 h-50 shrink-0'>
                    <Image
                        src={url}
                        alt={`Book cover for ${title}`}
                        fill
                        className='rounded'
                    />
            </div>
        );
    }

    return (
        <div className="flex items-center justify-center w-32 h-50 bg-gray-200 rounded">
            <p className="text-center text-gray-700 px-4 text-sm font-semibold">
                {title}
            </p>
        </div>
    );
}