<script lang="ts">
	import { Button } from "$lib/components/ui/button/index.js";
	import * as Card from "$lib/components/ui/card/index.js";
	import { Input } from "$lib/components/ui/input/index.js";
	import { Label } from "$lib/components/ui/label/index.js";
</script>

<Card.Root class="mx-auto max-w-sm">
	<Card.Header>
		<Card.Title class="text-2xl">Sign Up</Card.Title>
		<Card.Description>Enter an email and password below create an account</Card.Description>
	</Card.Header>
	<Card.Content>
		<div class="grid gap-4">
			<div class="grid gap-2">
				<Label for="email">Email</Label>
				<Input id="email" type="email" placeholder="m@example.com" required />
			</div>
			<div class="grid gap-2">
				<div class="flex items-center">
					<Label for="password">Password</Label>
				</div>
				<Input id="password" type="password" required />
			</div>
			<div class="grid gap-2">
				<div class="flex items-center">
					<Label for="password-confirm">Confirm Password</Label>
				</div>
				<Input id="password-password" type="passwordConfirmation" required />
			</div>
			<Button type="submit" class="w-full">Login</Button>
			<Button variant="outline" class="w-full">Login with Google</Button>
		</div>
		<div class="mt-4 text-center text-sm">
			Already have an account?
			<a href="/auth/login" class="underline">Log in</a>
		</div>
	</Card.Content>
</Card.Root>
