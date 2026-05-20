import Link from "next/link";
import ThemeToggle from "./theme/theme-toggle";

export default function Header() {
    return (

        <header className="w-full py-4 px-4 flex items-center justify-between">
            <Link href="/"><h1 className="text-2xl font-bold">ReadGood</h1></Link>
            <ThemeToggle />
        </header>
    )
}