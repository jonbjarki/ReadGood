import Image from "next/image";

export default function AvatarImage({ src, name, size }: { src: string | undefined, name: string | undefined, size: "small" | "large" }) {
    if (src) {
        const width = size == "small" ? 40 : 200;
        const height = size == "small" ? 40 : 200;

        return (
            <Image
                src={src}
                alt={name || "Avatar"}
                width={width}
                height={height}
                className="rounded-full"
            />
        );
    }

    return <DefaultAvatar name={name} />;
}

// Renders an avatar based on the user's initials
function DefaultAvatar({ name }: { name: string | undefined }) {
    const getInitials = (name: string) => {
        return name
            .split(" ")
            .map((word) => word[0])
            .join("")
            .toUpperCase()
            .slice(0, 2);
    };

    if (!name) {
        return (
            <div className="w-10 h-10 rounded-full bg-gray-300 flex items-center justify-center">
                <span className="text-gray-500 text-sm">👤</span>
            </div>
        );
    }

    const initials = getInitials(name);
    const colors = ["bg-blue-500", "bg-red-500", "bg-green-500", "bg-purple-500", "bg-yellow-500"];
    const colorIndex = initials.charCodeAt(0) % colors.length;

    return (
        <div className={`w-10 h-10 rounded-full ${colors[colorIndex]} flex items-center justify-center`}>
            <span className="text-white text-sm font-semibold">{initials}</span>
        </div>
    );
}