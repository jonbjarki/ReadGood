"use client"
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination"
import { SearchPageParamsType } from "@/types/search-page-types";
import { assert } from "console";
import { useSearchParams } from "next/navigation";

type PaginationLinkType = {
  page: number;
  link: string;
}

function generatePageLink(page: number, params: URLSearchParams): string {
  const newParams = new URLSearchParams(params.toString());
  newParams.set("page", page.toString());
  return "/books/search?" + newParams.toString();
}

function getPaginationLinks(currentPage: number, totalPages: number, params: URLSearchParams): PaginationLinkType[] {
  const links: PaginationLinkType[] = [];
  for (let page = Math.max(1, currentPage - 2); page <= Math.min(totalPages, currentPage + 1); page++) {
    links.push({ page, link: generatePageLink(page, params) });
  }
  return links;
}


export default function PaginationControls({parsedParams, totalPages}: {parsedParams: SearchPageParamsType, totalPages: number}) {
  const params = useSearchParams(); // Used for generating links, not for reading values since we already have parsedParams
  const { page } = parsedParams;
  const links = getPaginationLinks(page, totalPages, params);
  return (
    <Pagination>
      <PaginationContent>
        <PaginationItem>
          <PaginationPrevious href={links[0]?.link || "#"} />
        </PaginationItem>
        {links.map(({ page, link }) => (
          <PaginationItem key={page}>
            <PaginationLink href={link} isActive={page === parsedParams.page}>
              {page}
            </PaginationLink>
          </PaginationItem>
        ))}
        <PaginationItem>
          <PaginationEllipsis />
        </PaginationItem>
        <PaginationItem>
          <PaginationNext href={links[links.length-1]?.link || "#"} />
        </PaginationItem>
      </PaginationContent>
    </Pagination>
  )
}
